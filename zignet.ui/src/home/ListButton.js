import React, { Component } from 'react';

class ListButton extends Component {

  render() {
    return (
      <a href="/list" className="btn btn-primary" role="button" title="View results as list">List</a>
    );
  }
}

export default ListButton;
