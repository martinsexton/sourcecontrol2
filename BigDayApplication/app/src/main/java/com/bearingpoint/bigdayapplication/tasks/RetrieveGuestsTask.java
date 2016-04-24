package com.bearingpoint.bigdayapplication.tasks;

import android.content.Context;
import android.os.AsyncTask;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Martin.Sexton on 23/04/2016.
 */
public class RetrieveGuestsTask extends AsyncTask<String, String, List<Guest>> {
    IHandleListOfGuests cxt;

    public RetrieveGuestsTask(Context context) {
        cxt = (IHandleListOfGuests) context;
    }

    @Override
    protected List<Guest> doInBackground(String... params) {
        String SOAP_ACTION = "http://tempuri.org/IBigDaySOAPService/GetListOfGuests";
        String METHOD_NAME = "GetListOfGuests";
        String NAMESPACE = "http://tempuri.org/";
        String URL = "http://bomi4.cloudapp.net/Service1.svc?wsdl";
        List<Guest>guests = new ArrayList<Guest>();

        try {
            SoapObject Request = new SoapObject(NAMESPACE, METHOD_NAME);

            SoapSerializationEnvelope soapEnvelope = new SoapSerializationEnvelope(SoapEnvelope.VER10);
            soapEnvelope.dotNet = true;
            soapEnvelope.setOutputSoapObject(Request);

            HttpTransportSE transport = new HttpTransportSE(URL);

            transport.call(SOAP_ACTION, soapEnvelope);
            SoapObject response = (SoapObject) soapEnvelope.getResponse();

            for(int i=0;i<response.getPropertyCount();i++) {
                Object property = response.getProperty(i);
                if (property instanceof SoapObject) {
                    Guest g = new Guest();
                    SoapObject final_object = (SoapObject) property;
                    g.setIdentifier(Integer.parseInt(final_object.getPropertyAsString("Id")));
                    g.setFirstname(final_object.getPropertyAsString("Firstname"));
                    g.setSurname(final_object.getPropertyAsString("Surname"));
                    g.setStatus(final_object.getPropertyAsString("Status"));
                    g.setDietComment(final_object.getPropertyAsString("DietComment"));
                    g.setGuestName(final_object.getPropertyAsString("GuestName"));

                    guests.add(g);
                }
            }
        }
        catch (Exception ex) {
            String e = ex.getMessage();
        }
        return guests;
    }

    protected void onPostExecute(List<Guest> guests) {
        //notify GUI reg was successful
        String[] listData = new String[guests.size()];
        int index = 0;
        for(Guest g : guests){
            listData[index] = g.toString();
            index++;
        }
        cxt.HandleListOfGuests(listData);
    }
}
