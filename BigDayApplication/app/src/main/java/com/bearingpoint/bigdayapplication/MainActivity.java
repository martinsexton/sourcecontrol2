package com.bearingpoint.bigdayapplication;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.bearingpoint.bigdayapplication.tasks.Guest;
import com.bearingpoint.bigdayapplication.tasks.IHandleListOfGuests;
import com.bearingpoint.bigdayapplication.tasks.RetrieveGuestsTask;

import java.util.List;

public class MainActivity extends AppCompatActivity implements IHandleListOfGuests {
    ListView listView ;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

    public void HandleListOfGuests(String[] guests){
        listView = (ListView) findViewById(R.id.listOfGuests);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_list_item_1, android.R.id.text1, guests);

        listView.setAdapter(adapter);
    }

    public void DisplayGuests(View view){
        RetrieveGuestsTask getGuests = new RetrieveGuestsTask(this);
        getGuests.execute("guests");
    }
}
