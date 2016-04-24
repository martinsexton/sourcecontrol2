package com.bearingpoint.bigdayapplication.tasks;

/**
 * Created by Martin.Sexton on 23/04/2016.
 */
public class Guest {
    private int identifier;
    private String firstname;
    private String surname;
    private String status;
    private String guestName;
    private String dietComment;

    public int getIdentifier(){return identifier;}
    public String getFirstname(){return firstname;}
    public String getSurname(){return surname;}
    public String getStatus(){return status;}
    public String getGuestName(){return guestName;}
    public String getDietComment(){return dietComment;}

    public void setIdentifier(int id){identifier = id;}
    public void setFirstname(String fn){firstname=fn;}
    public void setSurname(String sn){surname=sn;}
    public void setStatus(String s){status=s;}
    public void setGuestName(String gn){guestName=gn;}
    public void setDietComment(String d){dietComment=d;}

    public String toString(){
        return getFirstname() + " " + getSurname() + " has " + getStatus();
    }
}
